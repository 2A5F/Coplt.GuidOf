﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Coplt.Analyzers.Utilities;
using Microsoft.CodeAnalysis;

namespace Coplt.Analyzers.Generators.Templates;

public readonly record struct GenBase(
    string RawFullName,
    NullableContextOptions Nullable,
    HashSet<string> Usings,
    ImmutableList<NameWrap>? Parents,
    NameWrap Target
)
{
    public string RawFullName { get; init; } = RawFullName;
    public string FileFullName { get; } = RawFullName.Replace('<', '[').Replace('>', ']');
}

public abstract class ATemplate(
    GenBase GenBase
)
{
    public GenBase GenBase = GenBase;

    public string FullName { get; } = $"global::{GenBase.RawFullName}";
    public string TypeName { get; } = GenBase.RawFullName.Split('.').Last();

    protected StringBuilder sb = new();

    protected abstract void DoGen();

    private void AddNullable()
    {
        if (GenBase.Nullable == NullableContextOptions.Disable)
        {
            sb.AppendLine("#nullable disable");
        }
        else if ((GenBase.Nullable & NullableContextOptions.Enable) != 0)
        {
            sb.AppendLine("#nullable enable");
        }
        else if ((GenBase.Nullable & NullableContextOptions.Warnings) != 0)
        {
            sb.AppendLine("#nullable warnings");
        }
        else if ((GenBase.Nullable & NullableContextOptions.Annotations) != 0)
        {
            sb.AppendLine("#nullable annotations");
        }
        sb.AppendLine();
    }

    private void AddUsings()
    {
        foreach (var use in GenBase.Usings)
        {
            sb.AppendLine(use);
        }
        if (GenBase.Usings.Count > 0) sb.AppendLine();
    }

    private void AddNameWrapPre()
    {
        if (GenBase.Parents != null)
        {
            foreach (var wrap in GenBase.Parents)
            {
                sb.Append(wrap.Code);
                sb.AppendLine(" {");
            }
        }
    }

    private void AddNameWrapPost()
    {
        if (GenBase.Parents != null)
        {
            foreach (var wrap in GenBase.Parents.Reverse())
            {
                sb.Append("} // ");
                sb.AppendLine(wrap.Code);
            }
        }
    }

    public string Gen()
    {
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine();
        AddNullable();
        AddUsings();
        AddNameWrapPre();
        sb.AppendLine();
        DoGen();
        sb.AppendLine();
        AddNameWrapPost();
        return sb.ToString();
    }
}
