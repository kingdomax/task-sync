﻿namespace TaskSync.Models.Response
{
    public class TestResponse
    {
        public string Message {  get; set; }

        public TestResponse(string message)
        {
            this.Message = message;
        }
    }
}
