import { Component } from "@angular/core";
import { UserService } from "src/app/services/user.service";

@Component({
    selector: 'register',
    templateUrl: './register.component.html'
})
export class RegisterComponent {
    userNameRegister: string = "";
    passwordRegister: string = "";

    constructor(
        private userService: UserService,
    ) { }

    register = () => {
        this.userService.create(this.userNameRegister, this.passwordRegister).subscribe((response: any) => {
            alert("User created");
        }, (err) => {
            alert("Error when creating user");
            console.log(err);
        });
    }
}