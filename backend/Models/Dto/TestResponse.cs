﻿namespace TaskSync.Models.Dto
{
    public class TestResponse
    {
        public string Message { get; set; }

        public TestResponse(string message)
        {
            Message = message;
        }
    }
}
