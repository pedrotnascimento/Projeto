import { Component } from '@angular/core';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ChatFrontEnd';
  isAuth: any = false;
  userLogged: any;

  constructor(
  ) { }
  ngOnInit() {
    this.isAuth = !!localStorage.getItem("accessToken");
    const user = localStorage.getItem("user");
    if (!!user) {
      this.userLogged = JSON.parse(user);
    }
  }

  login = ({ isAuthenticated, user }: any) => {
    this.isAuth = isAuthenticated;
    if (user) {
      this.userLogged = user;
      localStorage.setItem("user", JSON.stringify(user));
    }
  }
}
