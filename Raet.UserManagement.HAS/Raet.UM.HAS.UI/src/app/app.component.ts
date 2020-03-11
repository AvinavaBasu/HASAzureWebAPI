import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService } from './services/global.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  hideBack: boolean;
  private previousPageState: string;

  constructor(private router: Router, private globalService: GlobalService) {
    this.hideBack =  true;

    this.globalService.prevoiusState().subscribe(url => {
      if (url === undefined || url == null || url === '') {
        this.previousPageState = url;
        this.hideBack =  true;
      } else {
        this.previousPageState = url;
        this.hideBack =  false;
      }
    });
  }

  goToPreviuosState(): void {
    this.router.navigate([this.previousPageState]);
    this.previousPageState = '';
    this.hideBack =  true;
  }
}
