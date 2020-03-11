import {Injectable} from '@angular/core';

export class AppConfig {
  readonly testEndPoint: string;
}

export let APP_CONFIG: AppConfig;

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  constructor() {}

  public load() {
    return new Promise((resolve, reject) => {

      // this.http.get('/assets/config/config.json').subscribe((envResponse: any) => {
      //   const t = new AppConfig();
      //   APP_CONFIG = Object.assign(t, envResponse);
      //   resolve(true);
      // });
      APP_CONFIG = new AppConfig();
      resolve(true);

    });
  }

}
