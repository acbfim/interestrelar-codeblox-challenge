using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Interestrelar.Domain;

public class RetornoDto
{
    public string Message { get; set; } = "Erro ao realizar ação";
    public bool Success { get; set; } = false;
    public int StatusCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? TotalItems { get; set; } = 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Page { get; set; } = "0";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Object Data { get; set; }




    public static RetornoDto objectCreateSuccess(Object obj)
    {
        RetornoDto ret = new RetornoDto();
        ret.Message = "Criado com sucesso";
        ret.Success = true;
        ret.StatusCode = StatusCodes.Status201Created;
        ret.TotalItems = null;
        ret.Page = null;

        ret.Data = obj;
        return ret;
    }

    public static RetornoDto objectFoundSuccess(Object obj)
    {
        RetornoDto ret = new RetornoDto();
        ret.Message = "Localizado com sucesso";
        ret.Success = true;
        ret.StatusCode = StatusCodes.Status200OK;
        ret.TotalItems = 1;
        ret.Page = "1/1";

        ret.Data = obj;
        return ret;
    }

    public static RetornoDto objectsFoundSuccess(Object values, int skip, int take, long totalItems)
    {
        RetornoDto ret = new RetornoDto();
        ret.Message = "Localizado com sucesso";
        ret.Success = true;
        ret.StatusCode = StatusCodes.Status200OK;

        ret.TotalItems = totalItems;

        var actualPage = skip + 1;
        double totalPages = Math.Ceiling(totalItems / (double)take);
        var labelPage = actualPage > totalPages ? 0 : actualPage;


        ret.Page = (actualPage == 1 && labelPage == 0) ? "1/1" : $"{labelPage}/{totalPages}";

        ret.Data = values;
        return ret;
    }
}
