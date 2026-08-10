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

/**** CPU RADIOSITY (FASTER?) *****/

uniform sampler3D ambientShadowTexture;
uniform sampler3D radiosityTextureFlat;
uniform sampler3D radiosityTextureX;
uniform sampler3D radiosityTextureY;
uniform sampler3D radiosityTextureZ;
varying vec3 radiosityTextureCoord;
varying vec3 ambientShadowTextureCoord;
varying vec3 normalVarying;
uniform vec3 ambientColor;
uniform vec3 fogColor;

vec3 DecodeRadiosityValue(vec3 val){
	// reverse bias
	val *= 1023. / 1022.;
	val = (val * 2.) - 1.;
#if USE_RADIOSITY == 1
	// the low-precision radiosity texture uses a non-linear encoding
	val *= val * sign(val);
#endif
	return val;
}

vec3 Radiosity_Map(float detailAmbientOcclusion, float ssao) {
	vec3 col = DecodeRadiosityValue
	(texture3D(radiosityTextureFlat,
			   radiosityTextureCoord).xyz);
	vec3 normal = normalize(normalVarying);
	col += normal.x * DecodeRadiosityValue
	(texture3D(radiosityTextureX,
			   radiosityTextureCoord).xyz);
	col += normal.y * DecodeRadiosityValue
	(texture3D(radiosityTextureY,
			   radiosityTextureCoord).xyz);
	col += normal.z * DecodeRadiosityValue
	(texture3D(radiosityTextureZ,
			   radiosityTextureCoord).xyz);
	col = max(col, 0.);
	// Milder SSAO multiply keeps bounced light from collapsing to black
	col *= 1.35 * mix(0.55, 1.0, ssao);

	detailAmbientOcclusion = mix(detailAmbientOcclusion,
	                             detailAmbientOcclusion * ssao, 0.65);

	// ambient occlusion
	vec2 ambTexVal = texture3D(ambientShadowTexture, ambientShadowTextureCoord).xy;
	float amb = ambTexVal.x / max(ambTexVal.y, 0.25);
	amb = max(amb, 0.); // for some reason, mainTexture value becomes negative

	// Prefer sqrt mix for softer contact AO (less hard min crush)
	amb = mix(sqrt(amb * detailAmbientOcclusion), min(amb, detailAmbientOcclusion), 0.35);
	amb = mix(0.18, 1.0, clamp(amb, 0.0, 1.0));

	amb *= .85 - normalVarying.z * .15;
	// Slight warmth in ambient fill so voxels read less flat/cool-gray
	col += amb * ambientColor * vec3(1.05, 0.98, 0.90);

	return col;
}

vec3 BlurredReflection_Map(float detailAmbientOcclusion, vec3 direction, float ssao) {
	vec3 col = DecodeRadiosityValue
	(texture3D(radiosityTextureFlat,
			   radiosityTextureCoord).xyz);
	vec3 normal = normalize(normalVarying);
	col += normal.x * DecodeRadiosityValue
	(texture3D(radiosityTextureX,
			   radiosityTextureCoord).xyz);
	col += normal.y * DecodeRadiosityValue
	(texture3D(radiosityTextureY,
			   radiosityTextureCoord).xyz);
	col += normal.z * DecodeRadiosityValue
	(texture3D(radiosityTextureZ,
			   radiosityTextureCoord).xyz);
	col = max(col, 0.);
	col *= 1.35 * mix(0.55, 1.0, ssao);

	detailAmbientOcclusion = mix(detailAmbientOcclusion,
	                             detailAmbientOcclusion * ssao, 0.65);

	// ambient occlusion
	float amb = texture3D(ambientShadowTexture, ambientShadowTextureCoord).x;
	amb = max(amb, 0.); // for some reason, mainTexture value becomes negative
	amb = mix(amb, amb * amb, 0.55); // soften previous hard darken

	// Prefer sqrt mix for softer contact AO
	amb = mix(sqrt(amb * detailAmbientOcclusion), min(amb, detailAmbientOcclusion), 0.35);
	amb = mix(0.18, 1.0, clamp(amb, 0.0, 1.0));

	amb *= .85 - normalVarying.z * .15;

	return col + fogColor * amb * vec3(1.05, 0.98, 0.90);
}
