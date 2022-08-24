import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ChatRoomService } from './services/chatroom.service';
import { MessageService } from './services/message.service';
import { SignalrService } from './services/signalr.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ChatFrontEnd';
  chatName = "";
  userNameLogin = "";
  userNameRegister = "";
  passwordLogin = "";
  passwordRegister = "";
  isAuth: any = false;

  constructor(
    public signalRService: SignalrService,
    private http: HttpClient,
    private chatRoomService: ChatRoomService,
    private messageService: MessageService,
    private userService: UserService,
  ) { }
  ngOnInit() {
    this.isAuth = !!localStorage.getItem("accessToken");

    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
    this.startHttpRequest();
  }

  login = () => {
    this.userService.login(this.userNameLogin, this.passwordLogin).subscribe((response: any) => {
      console.log("response login", response);
      const isAuth = response.authenticated;
      this.isAuth = isAuth;
      if (isAuth) {
        const accessToken = response.accessToken;
        localStorage.setItem("accessToken", accessToken);
      }
    });
  }

  register = () => {
    this.userService.create(this.userNameRegister, this.passwordRegister).subscribe((response: any) => {
      console.log("response create user", response);
      
    });
  }

  createChat = () => {
    this.chatRoomService.create(this.chatName).subscribe(x => {
      console.log("created chatroom", x);
    });
  }

  private startHttpRequest = () => {
    this.http.get('http://localhost:5272/stocksocket')
      .subscribe(res => {
        console.log(res);
      })
  }
}
