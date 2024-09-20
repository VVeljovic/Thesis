import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection : signalR.HubConnection;
  constructor() { 
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5152/notifications').build()
  }
  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  receiveMessage(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('ReceiveNotification', (message: string) => {
        console.log(message)
        
        observer.next(message);
      });
    });
  }

}
