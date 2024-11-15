namespace ExpenseFlow.Communication.Response.Errors;
public class ResponseErrorModel
{
    public List<Object> Erros { get; set; } = [];

    public ResponseErrorModel(List<Object> modelErros)
    {
        Erros = modelErros;
    }

    public ResponseErrorModel(string modelErro)
    {
        Erros = [modelErro];
    }
}