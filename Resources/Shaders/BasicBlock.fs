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



varying vec4 color;
varying vec2 ambientOcclusionCoord;
varying vec2 detailCoord;
varying vec3 fogDensity;

uniform sampler2D ambientOcclusionTexture;
uniform sampler2D detailTexture;
uniform vec3 fogColor;

vec3 EvaluateSunLight();
vec3 EvaluateAmbientLight(float detailAmbientOcclusion);
//void VisibilityOfSunLight_Model_Debug();

void main() {
	// color is linear
	gl_FragColor = vec4(color.xyz, 1.);
	
	// Soften sun term so hard Lambert faces read less flat on voxels
	float sunTerm = mix(0.12, 1.0, clamp(color.w, 0.0, 1.0));
	vec3 shading = vec3(sunTerm);
	shading *= EvaluateSunLight();
	
	float ao = texture2D(ambientOcclusionTexture, ambientOcclusionCoord).x;
	ao = mix(0.4, 1.0, ao);
	
	shading += EvaluateAmbientLight(ao);
	
	// apply diffuse shading
	gl_FragColor.xyz *= shading;
	
	// apply fog
	gl_FragColor.xyz = mix(gl_FragColor.xyz, fogColor, fogDensity);
	
#if !LINEAR_FRAMEBUFFER
	// gamma correct
	gl_FragColor.xyz = sqrt(gl_FragColor.xyz);
#endif
}

