namespace MechanicShop.Domain.Common.Results
{
    public readonly record struct Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorKind Type { get; }

        private Error(string code, string desciption, ErrorKind type)
        {
            Code = code;
            Description = desciption;
            Type = type;
        }

        public static Error Failure(string code = nameof(Failure), string description = "General failure")
            => new(code, description, ErrorKind.Failure);
        public static Error UnExcpected(string code = nameof(UnExcpected), string description = "Unexpected error")
            => new(code, description, ErrorKind.UnExcpected);

        public static Error Validation(string code = nameof(Validation), string description = "Validation error")
            => new(code, description, ErrorKind.Validation);

        public static Error Conflict(string code = nameof(Conflict), string description = "Conflict error")
            => new(code, description, ErrorKind.Conflict);

        public static Error NotFound(string code = nameof(NotFound), string description = "Resource not found")
            => new(code, description, ErrorKind.NotFound);

        public static Error Unauthorized(string code = nameof(Unauthorized), string description = "Unauthorized access")
            => new(code, description, ErrorKind.Unauthorized);

        public static Error Forbidden(string code = nameof(Forbidden), string description = "Forbidden access")
            => new(code, description, ErrorKind.Forbidden);
    }

}
