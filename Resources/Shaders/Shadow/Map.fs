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


uniform sampler2D mapShadowTexture;

varying vec3 mapShadowCoord;

float VisibilityOfSunLight_Map() {
	float val = texture2D(mapShadowTexture, mapShadowCoord.xy).w;
	// Soft bias + partial penumbra for contact-shadow readability
	float dist = val - (mapShadowCoord.z - 0.00035);
	return clamp(dist * 180.0 + 0.15, 0.0, 1.0);
}
