﻿/*
 * MinIO .NET Library for Amazon S3 Compatible Cloud Storage, (C) 2020, 2021 MinIO, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Text;
using Minio.DataModel.Select;
using Minio.Helper;

namespace Minio.DataModel.Args;

public class SelectObjectContentArgs : EncryptionArgs<SelectObjectContentArgs>
{
    private readonly SelectObjectOptions selectOptions;

    public SelectObjectContentArgs()
    {
        RequestMethod = HttpMethod.Post;
        selectOptions = new SelectObjectOptions();
    }

    internal override void Validate()
    {
        base.Validate();
        if (string.IsNullOrEmpty(selectOptions.Expression))
            throw new InvalidOperationException("The Expression " + nameof(selectOptions.Expression) +
                                                " for Select Object Content cannot be empty.");

        if (selectOptions.InputSerialization is null || selectOptions.OutputSerialization is null)
            throw new InvalidOperationException(
                "The Input/Output serialization members for SelectObjectContentArgs should be initialized " +
                nameof(selectOptions.InputSerialization) + " " + nameof(selectOptions.OutputSerialization));
    }

    internal override HttpRequestMessageBuilder BuildRequest(HttpRequestMessageBuilder requestMessageBuilder)
    {
        requestMessageBuilder.AddQueryParameter("select", "");
        requestMessageBuilder.AddQueryParameter("select-type", "2");

        if (RequestBody.IsEmpty)
        {
            RequestBody = Encoding.UTF8.GetBytes(selectOptions.MarshalXML());
            requestMessageBuilder.SetBody(RequestBody);
        }

        requestMessageBuilder.AddOrUpdateHeaderParameter("Content-Md5",
            Utils.GetMD5SumStr(RequestBody.Span));

        return requestMessageBuilder;
    }

    public SelectObjectContentArgs WithExpressionType(QueryExpressionType e)
    {
        selectOptions.ExpressionType = e;
        return this;
    }

    public SelectObjectContentArgs WithQueryExpression(string expr)
    {
        selectOptions.Expression = expr;
        return this;
    }

    public SelectObjectContentArgs WithInputSerialization(SelectObjectInputSerialization serialization)
    {
        selectOptions.InputSerialization = serialization;
        return this;
    }

    public SelectObjectContentArgs WithOutputSerialization(SelectObjectOutputSerialization serialization)
    {
        selectOptions.OutputSerialization = serialization;
        return this;
    }

    public SelectObjectContentArgs WithRequestProgress(RequestProgress requestProgress)
    {
        selectOptions.RequestProgress = requestProgress;
        return this;
    }
}
