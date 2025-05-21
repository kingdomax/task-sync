import { NestFactory } from '@nestjs/core';
import { ValidationPipe } from '@nestjs/common';

import { AppModule } from './app.module';
import { TimingInterceptor } from './common/interceptors/timing.interceptor';
import { HttpExceptionFilter } from './common/exceptionfilters/http-exception.filter';

async function bootstrap() {
    const app = await NestFactory.create(AppModule);

    app.use(TimingInterceptor);
    app.useGlobalPipes(new ValidationPipe({ whitelist: true })); // enable DTO validation globally
    app.useGlobalFilters(new HttpExceptionFilter());

    await app.listen(process.env.PORT ?? 3000);
}
bootstrap();
