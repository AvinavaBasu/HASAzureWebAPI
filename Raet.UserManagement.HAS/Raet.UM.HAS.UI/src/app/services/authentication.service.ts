import { Injectable } from '@angular/core';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private pingIdentityUrl: string;

  constructor(private sessionService: SessionService) {
    // tslint:disable-next-line: max-line-length
    this.pingIdentityUrl = `https://identity.raettest.com/as/authorization.oauth2?response_type=token&client_id=Implicit&state=csnCxDsnQTLJwoU7EYs7QkxlC2kEuth5BP3eHaFc&redirect_uri=`;

  }

  verifyAuthentication(url: string): boolean {
    if (this.verifyToken(url)) {
      return true;
    }

    if (this.sessionService.Session.authenticated) {
      return true;
    }
    this.authenticate(url);
  }

  authenticate(redirectUrl?: string) {
    const port = window.location.port ? ':' + window.location.port : '';
    const hostUrl = window.location.protocol + '//' + window.location.hostname + port + '/';

    const pingUrl = this.pingIdentityUrl + encodeURIComponent(hostUrl);
    window.location.href = pingUrl;
  }

  private verifyToken(url: string): boolean {
    const tokenInfo = url.match(/\#(?:access_token)\=([\S\s]*?)\&/);

    if (!tokenInfo || !tokenInfo.length || tokenInfo.length < 2) { return false; }

    if (this.sessionService.Session.user) {
      this.sessionService.Session.user.accessToken = tokenInfo[1];
    } else {
      this.sessionService.Session.login(tokenInfo[1]);
    }

    return true;
  }

  public logout(): any {
    this.sessionService.Session.clear();
    const logoutUrl = 'https://identity.raettest.com/idp/startSLO.ping';
    window.location.href = logoutUrl;
  }
}