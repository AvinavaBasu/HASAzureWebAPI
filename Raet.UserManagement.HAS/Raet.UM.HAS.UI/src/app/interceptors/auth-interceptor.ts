import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injector, Injectable } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { SessionService } from '../services/session.service';
import { tap } from 'rxjs/operators';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private session: SessionService;

    constructor(private injector: Injector, private authService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.getAuthService().Session.authenticated) {
            request = request.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + this.getAuthService().Session.user.accessToken,
                    'x-raet-tenant-id': this.getAuthService().Session.user.tenantId
                },
            });
        }

        return next.handle(request).pipe(tap(() => { },
            (err: any) => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status !== 401) {
                        return throwError(err);
                    }
                }
                this.authService.logout();
            }));
    }

    getAuthService(): SessionService {
        if (typeof this.session === 'undefined') {
            this.session = this.injector.get(SessionService);
        }
        return this.session;
    }
}
