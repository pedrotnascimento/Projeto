import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { API_CONTROLER, API_STOCK_BOT } from '../../environments/environment';
import { ChatModel } from '../interfaces/chatInterface';
import { getHeaders } from './api-utils';



@Injectable({
  providedIn: 'root'
})
export class StockBotService {
  constructor(private http: HttpClient) { }

  sendCommand = (stockCode: string) => {
    return this.http.get(`${API_STOCK_BOT}/api/stockbot/${stockCode}`, getHeaders());
  }
}
