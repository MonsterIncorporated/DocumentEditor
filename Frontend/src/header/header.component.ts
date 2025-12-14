import {Component, inject, OnInit} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {Title} from '@angular/platform-browser';
import {FormControl, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {HttpClient, HttpResponse} from '@angular/common/http';
import {environment} from '../environments/environment';

@Component({
  selector: 'app-header',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {
  translation: TranslateService = inject(TranslateService);
  title: Title = inject(Title);
  form!: FormGroup;
  http: HttpClient = inject(HttpClient);
  constructor() {
    this.translation.stream('common.title').subscribe((res: string) => {
      this.title.setTitle(res)
    });
  }
  public ngOnInit() {
    this.form = new FormGroup({
      language: new FormControl('')
    });

    this.form.get('language')?.valueChanges.subscribe((lang: string) => {
      this.translation.use(lang)
    })

    this.http.get(environment.apiUrl + "/Example",{observe: 'response', responseType: 'text'}).subscribe((response: HttpResponse<string>) => {
      console.log(response.body);
    })
  }
}
