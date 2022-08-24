import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { ChatModel } from '../interfaces/chatInterface';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public data: ChatModel[] = [];
  private hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5272/stocksocket')
    .build();

  public startConnection = () => {
    //this.hubConnection = new signalR.HubConnectionBuilder()
    //  .withUrl('ws://localhost:5272/stocksocket')
    //  .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

    public addTransferChartDataListener = () => {
      this.hubConnection.on('messageReceived', (data) => {
      this.data = data;
      console.log(data);
    });
  }
}
