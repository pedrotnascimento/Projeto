import { Component, EventEmitter, Output } from "@angular/core";
import { UserService } from "src/app/services/user.service";

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    userNameLogin = "";
    passwordLogin = "";
    @Output() onAuthenticated = new EventEmitter();
    constructor(
        private userService: UserService

    ) { }

    login = () => {
        this.userService.login(this.userNameLogin, this.passwordLogin).subscribe((response: any) => {
            const isAuthenticated = response.authenticated;
            if (isAuthenticated) {
                const accessToken = response.accessToken;
                if (isAuthenticated) {
                    localStorage.setItem("accessToken", accessToken);
                    this.userService.getLoggedUser().subscribe(user => {
                        this.onAuthenticated.emit({ isAuthenticated, user });

                    })
                }
                return;
            }
            this.onAuthenticated.emit({ isAuthenticated });
        });
    }

}