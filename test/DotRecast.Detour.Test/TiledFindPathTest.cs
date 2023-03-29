/*
recast4j Copyright (c) 2015-2019 Piotr Piastucki piotr@jtilia.org

This software is provided 'as-is', without any express or implied
warranty.  In no event will the authors be held liable for any damages
arising from the use of this software.
Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:
1. The origin of this software must not be misrepresented; you must not
 claim that you wrote the original software. If you use this software
 in a product, an acknowledgment in the product documentation would be
 appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be
 misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.
*/

using System.Collections.Generic;
using DotRecast.Core;
using NUnit.Framework;

namespace DotRecast.Detour.Test;

public class TiledFindPathTest
{
    private static readonly Status[] STATUSES = { Status.SUCCSESS };

    private static readonly long[][] RESULTS =
    {
        new[]
        {
            281475015507969L, 281475014459393L, 281475014459392L, 281475006070784L,
            281475005022208L, 281475003973636L, 281475012362240L, 281475012362241L, 281475012362242L, 281475003973634L,
            281475003973635L, 281475003973633L, 281475002925059L, 281475002925057L, 281475002925056L, 281474998730753L,
            281474998730754L, 281474994536450L, 281474994536451L, 281474994536452L, 281474994536448L, 281474990342146L,
            281474990342145L, 281474991390723L, 281474991390724L, 281474991390725L, 281474987196418L, 281474987196417L,
            281474988244996L, 281474988244995L, 281474988244997L, 281474985099266L
        }
    };

    protected static readonly long[] START_REFS = { 281475015507969L };
    protected static readonly long[] END_REFS = { 281474985099266L };
    protected static readonly Vector3f[] START_POS = { Vector3f.Of(39.447338f, 9.998177f, -0.784811f) };
    protected static readonly Vector3f[] END_POS = { Vector3f.Of(19.292645f, 11.611748f, -57.750366f) };

    protected NavMeshQuery query;
    protected NavMesh navmesh;

    [SetUp]
    public void setUp()
    {
        navmesh = createNavMesh();
        query = new NavMeshQuery(navmesh);
    }

    protected NavMesh createNavMesh()
    {
        return new TestTiledNavMeshBuilder().getNavMesh();
    }

    [Test]
    public void testFindPath()
    {
        QueryFilter filter = new DefaultQueryFilter();
        for (int i = 0; i < START_REFS.Length; i++)
        {
            long startRef = START_REFS[i];
            long endRef = END_REFS[i];
            Vector3f startPos = START_POS[i];
            Vector3f endPos = END_POS[i];
            Result<List<long>> path = query.findPath(startRef, endRef, startPos, endPos, filter);
            Assert.That(path.status, Is.EqualTo(STATUSES[i]));
            Assert.That(path.result.Count, Is.EqualTo(RESULTS[i].Length));
            for (int j = 0; j < RESULTS[i].Length; j++)
            {
                Assert.That(RESULTS[i][j], Is.EqualTo(path.result[j]));
            }
        }
    }
}