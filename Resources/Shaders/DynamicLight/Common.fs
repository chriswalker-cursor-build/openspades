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


// Common code for dynamic light rendering


// -- shadowing

float VisibilityOfLight_Map();

float VisibilityOfLight() {
	return VisibilityOfLight_Map();
}

float EvaluateDynamicLightShadow(){
	return VisibilityOfLight();
}

// -- lighting (without bumpmapping)

uniform vec3 dynamicLightColor;
uniform float dynamicLightRadius;
uniform float dynamicLightRadiusInversed;
uniform sampler2D dynamicLightProjectionTexture;

varying vec3 lightPos;
varying vec3 lightNormal;
varying vec3 lightTexCoord;

vec3 EvaluateDynamicLightNoBump() {
	vec3 texValue = texture2DProj(dynamicLightProjectionTexture, lightTexCoord).xyz;

	if (lightTexCoord.z < 0. || any(lessThan(lightTexCoord.xy, vec2(0.0))) ||
	    any(greaterThan(lightTexCoord.xy, vec2(lightTexCoord.z))))
		discard;

	// diffuse lighting — soft wrap so voxels pick up a little fill at grazing angles
	float ndotl = dot(normalize(lightPos), normalize(lightNormal));
	float intensity = clamp(ndotl * 0.85 + 0.15, 0.0, 1.0);
	if(ndotl < -0.2) discard;
	
	// attenuation — smoother falloff near the radius edge
	float distance = length(lightPos);
	if(distance >= dynamicLightRadius) discard;
	distance *= dynamicLightRadiusInversed;
	distance = max(1. - distance, 0.);
	float att = distance * distance * (1.0 + 0.35 * distance);
	
	// apply attenuation
	intensity *= att;

	// Warm dynamic lights (muzzle / grenade / torch feel less sterile)
	vec3 warmColor = dynamicLightColor * vec3(1.12, 0.96, 0.82);

	// TODO: specular lighting?
	return warmColor * intensity * EvaluateDynamicLightShadow() * texValue;
}

// TODO: bumpmapping variant (requires tangent vector)

