import { Component, OnInit, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'has-subheader',
  templateUrl: './subheader.component.html',
  styleUrls: ['./subheader.component.scss']
})
export class SubheaderComponent implements OnInit {

  @Input() headerTemplate: TemplateRef<any>;
  @Input() actionTemplate: TemplateRef<any>;

  constructor() { }

  ngOnInit() {
  }

}
