import {
    CallHandler,
    ExecutionContext,
    Injectable,
    NestInterceptor,
} from '@nestjs/common';
import { Observable, tap } from 'rxjs';
import { Request } from 'express';

@Injectable()
export class TimingInterceptor implements NestInterceptor {
    intercept(context: ExecutionContext, next: CallHandler): Observable<any> {
        const start = Date.now();
        return next.handle().pipe(
            tap(() => {
                const duration = Date.now() - start;
                const req: Request = context.switchToHttp().getRequest();
                console.log(`[TimingInterceptor] ${duration}ms`);
                console.log(`-------------------`);
            })
        );
    }
}
