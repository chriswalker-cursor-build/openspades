/*
 Copyright (c) 2013 yvt

 This file is part of OpenSpades.

 OpenSpades is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 OpenSpades is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with OpenSpades.  If not, see <http://www.gnu.org/licenses/>.

 */



uniform vec3 fogColor;
varying float hemisphereLighting;

vec3 Radiosity_Map(float detailAmbientOcclusion, float ssao) {
	// Soften AO floor + warm the no-radiosity fill so voxels stay readable
	float ao = mix(0.22, 1.0, clamp(detailAmbientOcclusion * ssao, 0.0, 1.0));
	return mix(fogColor, vec3(1.05, 0.98, 0.90), 0.5) *
	(0.55 * ao * hemisphereLighting);
}

vec3 BlurredReflection_Map(float detailAmbientOcclusion, vec3 direction, float ssao) {
	float ao = mix(0.22, 1.0, clamp(detailAmbientOcclusion * ssao, 0.0, 1.0));
	return fogColor * ((direction.z * -0.5 + 0.5) * ao) * vec3(1.05, 0.98, 0.90);
}
