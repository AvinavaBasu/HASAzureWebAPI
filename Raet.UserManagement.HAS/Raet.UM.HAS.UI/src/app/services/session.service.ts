import { Injectable } from '@angular/core';
import * as jwtDecode from 'jwt-decode';

export class SessionInfo {
  user: LoggedUser;

  login(accessToken: string): boolean {

    let decodedToken: TokenInfo;

    try {
      decodedToken = jwtDecode<TokenInfo>(accessToken);
    } catch {
      return null;
    }

    this.user = {
      userId: decodedToken.uid,
      tenantId: decodedToken.tid,
      sourceSystem: decodedToken.sourcesystem,
      sourceUserId: decodedToken.sourceid,
      accessToken
    };
    return true;
  }

  clear(): void {
    this.user = null;
  }

  get authenticated(): boolean {
    return this.user != null;
  }
}

export class TokenInfo {
  client_id: string;
  exp: number;
  iss: string;
  sourceid: string;
  sourcesystem: string;
  tid: string;
  uid: string;
}

export class LoggedUser {
  sourceSystem: string;
  sourceUserId: string;
  tenantId: string;
  userId: string;
  accessToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor() {
    this.session = new SessionInfo();
  }

  private session: SessionInfo;

  get Session(): SessionInfo {
    return this.session;
  }
}