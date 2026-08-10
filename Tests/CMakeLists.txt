# Minimal Catch2 characterisation suite (not full game coverage).
# Headless; no GPU/OpenGL. Linked only against header-pure Core helpers.

set(OPENSPADES_TEST_SOURCES
	MathTests.cpp
)

add_executable(openspades_unit_tests ${OPENSPADES_TEST_SOURCES})

target_include_directories(openspades_unit_tests PRIVATE
	${CMAKE_SOURCE_DIR}/Sources
	${CMAKE_CURRENT_SOURCE_DIR}/third_party
)

# Match the game's common Linux baseline without pulling the full OpenSpades target.
set_target_properties(openspades_unit_tests PROPERTIES
	CXX_STANDARD 14
	CXX_STANDARD_REQUIRED ON
	RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin
)

add_test(NAME openspades_unit_tests COMMAND openspades_unit_tests)
