import { NestFactory } from '@nestjs/core';
import { ValidationPipe } from '@nestjs/common';

import { AppModule } from './app.module';
import { AuthGuard } from './common/guards/auth.guard';
import { TimingInterceptor } from './common/interceptors/timing.interceptor';
import { HttpExceptionFilter } from './common/exceptionfilters/http-exception.filter';
import { RequestLoggerMiddleware } from './common/middlewares/request-logger.middleware';

async function bootstrap() {
    const app = await NestFactory.create(AppModule);

    app.use(new RequestLoggerMiddleware()); // only place to register middleware, cannot register per route via @attribution

    app.useGlobalGuards(new AuthGuard());
    app.useGlobalInterceptors(new TimingInterceptor());
    app.useGlobalPipes(new ValidationPipe({ whitelist: true }));
    app.useGlobalFilters(new HttpExceptionFilter());

    await app.listen(process.env.PORT ?? 3000);
}
bootstrap();
