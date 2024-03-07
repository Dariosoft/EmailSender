﻿namespace Dariosoft.Framework
{
    public class ListReply<T> : Reply
    {
        private int pageNumber = 1, pageSize = 10, totalItems = 0;

        public required IEnumerable<T> Data { get; init; }

        public int PageNumber
        {
            get => pageNumber;
            init => pageNumber = value < 1 ? 1 : pageNumber;
        }

        public int PageSize
        {
            get => pageSize;
            init => pageSize = value < 1 ? 1 : pageSize;
        }

        public int TotalItems
        {
            get => totalItems;
            init => totalItems = value < 0 ? 0 : totalItems;
        }

        public static ListReply<T> Success(IEnumerable<T> data) => Success(data, data.Count());

        public static ListReply<T> Success(IEnumerable<T> data, int totalItems, int pageNumber = 1, int pageSize = 15)
            => new ListReply<T> { IsSuccessful = true, Data = data, TotalItems = totalItems, PageNumber = pageNumber, PageSize = pageSize };

        public new static ListReply<T> Fail(string message, string? code = null)
            => new ListReply<T> { IsSuccessful = false, Data = [], Reasons = [new Reason { Text = message, Code = code }] };

        public static ListReply<T> From(Reply other, IEnumerable<T> data)
            => new ListReply<T> { IsSuccessful = other.IsSuccessful, Data = data, Reasons = other.Reasons };

        public static ListReply<T> From(Reply other)
            => new ListReply<T> { IsSuccessful = other.IsSuccessful, Data = [], Reasons = other.Reasons };
    }
}
