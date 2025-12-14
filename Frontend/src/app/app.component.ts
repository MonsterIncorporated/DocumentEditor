import {Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TranslateDirective} from '@ngx-translate/core';
import {HeaderComponent} from '../header/header.component';
import {environment} from '../environments/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TranslateDirective, HeaderComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {
  public URL: string;
  constructor() {
    this.URL = environment.apiUrl;
  }
}
