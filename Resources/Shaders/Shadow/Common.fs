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


// Common code for sunlight shadow rendering


#if USE_SSAO
uniform sampler2D ssaoTexture;
uniform vec2 ssaoTextureUVScale;
#endif

float VisibilityOfSunLight_Map();
float VisibilityOfSunLight_Model();
vec3 Radiosity_Map(float detailAmbientOcclusion, float ssao);
vec3 BlurredReflection_Map(float detailAmbientOcclusion, vec3 direction, float ssao);

float VisibilityOfSunLight() {
	return VisibilityOfSunLight_Map() *
	VisibilityOfSunLight_Model();
}

vec3 EvaluateSunLight(){
	// Warm directional key — less flat gray sunlight on voxels
	return vec3(0.68, 0.58, 0.46) * VisibilityOfSunLight();
}

vec3 EvaluateAmbientLight(float detailAmbientOcclusion) {
#if USE_SSAO
    float ssao = texture2D(ssaoTexture, gl_FragCoord.xy * ssaoTextureUVScale).x;
    // Soften SSAO influence so ambient fill survives in creases
    ssao = mix(1.0, ssao, 0.78);
#else
    float ssao = 1.0;
#endif
	return Radiosity_Map(detailAmbientOcclusion, ssao);
}

vec3 EvaluateDirectionalAmbientLight(float detailAmbientOcclusion, vec3 direction) {
#if USE_SSAO
    float ssao = texture2D(ssaoTexture, gl_FragCoord.xy * ssaoTextureUVScale).x;
    ssao = mix(1.0, ssao, 0.78);
#else
    float ssao = 1.0;
#endif
    return BlurredReflection_Map(detailAmbientOcclusion, direction, ssao);
}
