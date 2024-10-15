
using AccommodationService.Application.Dtos;
using System.Text.Json;

public class ReservationService : IReservationService
{
    private ReservationContext _reservationContext;

    private IRabbitMQProducer<TransactionRequestDto> _producerForTransactionRequest;

    private IRabbitMQProducer<TransactionResponseDto> _producerForTransactionResponse;

    public ReservationService(ReservationContext reservationContext, IRabbitMQProducer<TransactionRequestDto> producerForTransactionRequest, IRabbitMQProducer<TransactionResponseDto> producerForTransactionFailedMessage)
    {
        _reservationContext = reservationContext;
        _producerForTransactionRequest = producerForTransactionRequest;
        _producerForTransactionResponse = producerForTransactionFailedMessage;
    }
    public async Task CreateReservationAsync(CreateReservationCommand createReservationCommand)
    {
        var reservation = CreateReservationCommand.MapToReservation(createReservationCommand);
        await _reservationContext.InsertReservationAsync(reservation);
    }

    public async Task HandleMessageAsync<T>(T message, string queueName)
    {
        if (queueName == "reservation_request")
        {
            await TransactionRequestAsync(message, "reservation_request");
            return;
        }
        else if (queueName == "payment_failed")
        {
            await TransactionFailedAsync(message, "payment_failed");
            return;
        }
        else
        {
            await TransactionSuccessAsync(message, "payment_success");
            return;
        }

    }

    private async Task TransactionRequestAsync<T>(T message, string queueName)
    {
        var reservationRequest = message as TransactionRequestDto;
        var reservation = await _reservationContext.FindOneAsync(reservationRequest);
        if (reservation == null)
        {
            var reservationEntity = new Reservation()
            {
                CheckInDate = reservationRequest.DateFrom,
                CheckOutDate = reservationRequest.DateTo,
                TotalAmount = reservationRequest.TotalAmount,
                Status = "Pending",
                AccommodationId = reservationRequest.AccommodationId,
                UserId = reservationRequest.UserId

            };
            await _reservationContext.InsertReservationAsync(reservationEntity);
            reservationRequest.ReservationId = reservationEntity.Id.ToString();
            await _producerForTransactionRequest.PublishMessageAsync(reservationRequest, "payment_request");
        }
        else
        {
            var transactionFailedMessageDto = new TransactionResponseDto()
            {
                TransactionId = reservationRequest.TransactionId,
                Message = "The accommodation is already booked for the selected dates.",
                TransactionStatus = "Failed"

            };
            await _producerForTransactionResponse.PublishMessageAsync(transactionFailedMessageDto, "reservation_failed");
        }

    }

    private async Task TransactionSuccessAsync<T>(T message, string queueName)
    {
        var transactionResponse = message as TransactionResponseDto;
        transactionResponse.TransactionStatus = "Success";
        Console.WriteLine(transactionResponse.Message);
        await _reservationContext.UpdateReservationAsync(transactionResponse.ReservationId, transactionResponse.TransactionStatus);
        await _producerForTransactionResponse.PublishMessageAsync(transactionResponse, "reservation_success");
    }

    private async Task TransactionFailedAsync<T>(T message, string queueName)
    {
        var transactionResponse = message as TransactionResponseDto;
        Console.WriteLine(transactionResponse.Message);
        await _reservationContext.UpdateReservationAsync(transactionResponse.ReservationId, transactionResponse.TransactionStatus);
        transactionResponse.TransactionStatus = "Failed";
        await _producerForTransactionResponse.PublishMessageAsync(transactionResponse, "reservation_failed");
    }


    async Task<IEnumerable<ReservationDto>> IReservationService.GetReservationsByUserId(string userId, int pageSize=5, int pageNumber=1, string status="Success")
    {
        return await _reservationContext.GetReservationsByUserIdAsync(userId,pageSize,pageNumber,status);
    }
}