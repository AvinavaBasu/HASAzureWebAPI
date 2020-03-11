import { Injectable } from '@angular/core';
import { HttpClient, HttpBackend } from '@angular/common/http';

export class Configs {
    testEndPoint: any;
    reportingBaseUrl: any;
}

@Injectable({
    providedIn: 'root'
})


export class ConfigService {
    private config: Configs;

    constructor(private http: HttpClient) {

    }

    loadConfig() {
        const promise = this.http.get('./assets/config/config.json')
        .toPromise()
        .then(data => {
          Object.assign(this, data);
          this.config = data as Configs;
          return data;
        });
        return promise;
    }

    getConfig() {
        return this.config;
    }
}
