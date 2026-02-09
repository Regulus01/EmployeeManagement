namespace EmployeeManagement.Application.Common
{
    /// <summary>
    /// Representa o resultado de uma operação, indicando sucesso ou falha
    /// e contendo possíveis mensagens de erro.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indica se a operação foi concluída com sucesso.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Lista de erros retornados pela operação quando ela falha.
        /// </summary>
        public string[] Errors { get; }

        /// <summary>
        /// Construtor base para criação de resultados.
        /// </summary>
        /// <param name="isSuccess">Indica se a operação foi bem-sucedida.</param>
        /// <param name="errors">Lista de erros associados à operação.</param>
        protected Result(bool isSuccess, string[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors ?? Array.Empty<string>();
        }

        /// <summary>
        /// Cria um resultado de falha com uma lista de erros.
        /// </summary>
        /// <param name="errors">Mensagens de erro.</param>
        public static Result Failure(string[] errors) => new(false, errors);

        /// <summary>
        /// Cria um resultado de sucesso contendo um valor.
        /// </summary>
        /// <typeparam name="T">Tipo do valor retornado.</typeparam>
        /// <param name="value">Valor retornado pela operação.</param>
        public static Result<T> Success<T>(T value) => new(value, true, []);

        /// <summary>
        /// Cria um resultado de falha tipado contendo erros.
        /// </summary>
        /// <typeparam name="T">Tipo esperado do valor.</typeparam>
        /// <param name="errors">Mensagens de erro.</param>
        public static Result<T> Failure<T>(string[] errors) => new(default, false, errors);
    }

    /// <summary>
    /// Representa o resultado de uma operação que retorna um valor.
    /// </summary>
    /// <typeparam name="T">Tipo do valor retornado.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Valor retornado pela operação quando ela é bem-sucedida.
        /// Será <c>null</c> em caso de falha.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Construtor interno para criação de resultados tipados.
        /// </summary>
        /// <param name="value">Valor retornado.</param>
        /// <param name="isSuccess">Indica se a operação foi bem-sucedida.</param>
        /// <param name="errors">Lista de erros.</param>
        internal Result(T? value, bool isSuccess, string[] errors) : base(isSuccess, errors)
        {
            Value = value;
        }
    }
}