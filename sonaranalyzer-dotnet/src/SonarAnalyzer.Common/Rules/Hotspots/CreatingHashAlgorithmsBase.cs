/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2020 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using SonarAnalyzer.Helpers;

namespace SonarAnalyzer.Rules
{
    public abstract class CreatingHashAlgorithmsBase<TSyntaxKind> : SonarDiagnosticAnalyzer
        where TSyntaxKind : struct
    {
        protected const string DiagnosticId = "S4790";
        protected const string MessageFormat = "Make sure that hashing data is safe here.";

        protected ObjectCreationTracker<TSyntaxKind> ObjectCreationTracker { get; set; }

        protected InvocationTracker<TSyntaxKind> InvocationTracker { get; set;  }

        protected BaseTypeTracker<TSyntaxKind> BaseTypeTracker { get; set; }

        protected override void Initialize(SonarAnalysisContext context)
        {
            ObjectCreationTracker.Track(context,
                ObjectCreationTracker.WhenDerivesFrom(KnownType.System_Security_Cryptography_HashAlgorithm));

            InvocationTracker.Track(context,
                InvocationTracker.MethodNameIs("Create"),
                InvocationTracker.MethodReturnTypeIs(KnownType.System_Security_Cryptography_HashAlgorithm));

            BaseTypeTracker.Track(context,
                BaseTypeTracker.MatchSubclassesOf(KnownType.System_Security_Cryptography_HashAlgorithm));
        }
    }
}
