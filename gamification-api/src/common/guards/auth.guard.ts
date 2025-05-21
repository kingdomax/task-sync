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
        const isAuthorized = request.headers['x-fake-auth'] === 'yes';
        if (!isAuthorized) {
            throw new UnauthorizedException('Missing x-fake-auth header');
        }
        return true;
    }
}
