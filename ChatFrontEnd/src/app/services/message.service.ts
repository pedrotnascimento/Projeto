import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { API_CONTROLER } from '../../environments/environment';
import { ChatModel } from '../interfaces/chatInterface';
import { getHeaders } from './api-utils';



@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(private http: HttpClient) { }

  create = (obj: any) => {
    const body = obj;
    return this.http.post(`${API_CONTROLER}/api/message/create`, body, getHeaders());
  }

  list = (chatId: number) => {
    return this.http.get(`${API_CONTROLER}/api/message/list/${chatId}`, getHeaders());
  }
}
