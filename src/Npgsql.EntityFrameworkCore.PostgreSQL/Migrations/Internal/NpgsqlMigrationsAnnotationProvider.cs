﻿#region License
// The PostgreSQL License
//
// Copyright (C) 2016 The Npgsql Development Team
//
// Permission to use, copy, modify, and distribute this software and its
// documentation for any purpose, without fee, and without a written
// agreement is hereby granted, provided that the above copyright notice
// and this paragraph and the following two paragraphs appear in all copies.
//
// IN NO EVENT SHALL THE NPGSQL DEVELOPMENT TEAM BE LIABLE TO ANY PARTY
// FOR DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES,
// INCLUDING LOST PROFITS, ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS
// DOCUMENTATION, EVEN IF THE NPGSQL DEVELOPMENT TEAM HAS BEEN ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
// THE NPGSQL DEVELOPMENT TEAM SPECIFICALLY DISCLAIMS ANY WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE. THE SOFTWARE PROVIDED HEREUNDER IS
// ON AN "AS IS" BASIS, AND THE NPGSQL DEVELOPMENT TEAM HAS NO OBLIGATIONS
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
#endregion

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal
{
    public class NpgsqlMigrationsAnnotationProvider : MigrationsAnnotationProvider
    {
        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            if (property.ValueGenerated == ValueGenerated.OnAdd &&
                property.ClrType.IsIntegerForSerial()) {
                yield return new Annotation(NpgsqlAnnotationNames.Prefix + NpgsqlAnnotationNames.Serial, true);
            }

            // TODO: Named sequences

            // TODO: We don't support ValueGenerated.OnAddOrUpdate, so should we throw an exception?
            // Other providers don't seem to...
        }

        public override IEnumerable<IAnnotation> For(IIndex index)
        {
            if (index.Npgsql().Method != null)
            {
                yield return new Annotation(
                     NpgsqlAnnotationNames.Prefix + NpgsqlAnnotationNames.IndexMethod,
                     index.Npgsql().Method);
            }
        }
    }
}