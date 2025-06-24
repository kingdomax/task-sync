import {
    CanActivate,
    ExecutionContext,
    Injectable,
    UnauthorizedException,
} from '@nestjs/common';
import { Request } from 'express';

@Injectable()
export class AuthGuard implements CanActivate {
    canActivate(context: ExecutionContext): boolean {
        const request: Request = context.switchToHttp().getRequest();
        const isAuthorized = request.headers['x-gamapi-auth'] === 'true';
        if (!isAuthorized) {
            throw new UnauthorizedException(
                '[AuthGuard] Missing x-gamapi-auth header'
            );
        }
        return true;
    }
}
