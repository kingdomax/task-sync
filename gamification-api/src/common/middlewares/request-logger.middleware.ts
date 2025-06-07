//import { Injectable, NestMiddleware } from '@nestjs/common';
import { Request, Response, NextFunction } from 'express';

//@Injectable() need to inject at module
//export class RequestLoggerMiddleware implements NestMiddleware {
//    use(req: Request, res: Response, next: NextFunction) {
//        console.log(
//            `[${new Date().toISOString()}] ${req.method} ${req.originalUrl}`
//        );
//        next();
//    }
//}

export function requestLoggerMiddleware(
    req: Request,
    res: Response,
    next: NextFunction
) {
    console.log(`-------------------`);
    console.log(`[RequestLoggerMiddleware] ${req.method} ${req.originalUrl}`);
    next();
}
