/*
 Characterisation tests for pure helpers in Sources/Core/Math.h (header-inline).
 Minimal factory suite — not full game coverage. No GPU/OpenGL.
 */

#define CATCH_CONFIG_MAIN
#include "catch.hpp"

#include <Core/Math.h>
#include <string>
#include <vector>

using spades::IntVector3;
using spades::Vector3;

TEST_CASE("IntVector3::Dot is the sum of component products", "[math][intvector3]") {
	IntVector3 a = IntVector3::Make(1, 2, 3);
	IntVector3 b = IntVector3::Make(4, 5, 6);
	REQUIRE(IntVector3::Dot(a, b) == 32);
}

TEST_CASE("IntVector3 Manhattan and Chebyshev lengths", "[math][intvector3]") {
	IntVector3 v = IntVector3::Make(-3, 4, -5);
	REQUIRE(v.GetManhattanLength() == 12);
	REQUIRE(v.GetChebyshevLength() == 5);
}

TEST_CASE("IntVector3 arithmetic and equality", "[math][intvector3]") {
	IntVector3 a = IntVector3::Make(1, 2, 3);
	IntVector3 b = IntVector3::Make(10, 20, 30);
	REQUIRE(a + b == IntVector3::Make(11, 22, 33));
	REQUIRE(b - a == IntVector3::Make(9, 18, 27));
	REQUIRE(a * 2 == IntVector3::Make(2, 4, 6));
	REQUIRE(-a == IntVector3::Make(-1, -2, -3));
}

TEST_CASE("Vector3 Dot and Cross", "[math][vector3]") {
	Vector3 x = Vector3::Make(1.f, 0.f, 0.f);
	Vector3 y = Vector3::Make(0.f, 1.f, 0.f);
	REQUIRE(Vector3::Dot(x, y) == Approx(0.f));
	Vector3 z = Vector3::Cross(x, y);
	REQUIRE(z.x == Approx(0.f));
	REQUIRE(z.y == Approx(0.f));
	REQUIRE(z.z == Approx(1.f));
}

TEST_CASE("Vector3 length and normalize", "[math][vector3]") {
	Vector3 v = Vector3::Make(3.f, 0.f, 4.f);
	REQUIRE(v.GetPoweredLength() == Approx(25.f));
	REQUIRE(v.GetLength() == Approx(5.f));
	Vector3 n = v.Normalize();
	REQUIRE(n.GetLength() == Approx(1.f));
	REQUIRE(n.x == Approx(0.6f));
	REQUIRE(n.z == Approx(0.8f));
}

TEST_CASE("Vector3 zero Normalize stays zero", "[math][vector3]") {
	Vector3 z = Vector3::Make(0.f, 0.f, 0.f);
	Vector3 n = z.Normalize();
	REQUIRE(n.x == Approx(0.f));
	REQUIRE(n.y == Approx(0.f));
	REQUIRE(n.z == Approx(0.f));
}

TEST_CASE("CodePointToUTF8 encodes ASCII and 2-byte Latin", "[math][utf8]") {
	std::string out;
	spades::CodePointToUTF8(std::back_inserter(out), 0x41); // 'A'
	REQUIRE(out == "A");

	out.clear();
	spades::CodePointToUTF8(std::back_inserter(out), 0x00E9); // é
	REQUIRE(out.size() == 2);
	REQUIRE(static_cast<unsigned char>(out[0]) == 0xc3);
	REQUIRE(static_cast<unsigned char>(out[1]) == 0xa9);
}

TEST_CASE("MakeVector3 from IntVector3 preserves components", "[math][vector3]") {
	IntVector3 i = IntVector3::Make(-1, 2, -3);
	Vector3 v = spades::MakeVector3(i);
	REQUIRE(v.x == Approx(-1.f));
	REQUIRE(v.y == Approx(2.f));
	REQUIRE(v.z == Approx(-3.f));
}
