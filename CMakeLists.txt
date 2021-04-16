cmake_minimum_required(VERSION 3.11)
set(PROJECT_NAME Potato)
project(${PROJECT_NAME})

function(watch)
    set_property(
            DIRECTORY
            APPEND
            PROPERTY CMAKE_CONFIGURE_DEPENDS ${ARGV}
    )
endfunction()

set(CMAKE_POLICY_DEFAULT_CMP0012 NEW)
set(CMAKE_CXX_STANDARD 14)

if(MSVC)
else()
        list(APPEND CMAKE_CXX_FLAGS "-Wall -Wextra -Wno-unused-variable -Wno-unused-parameter -O3")
endif(MSVC)


file(GLOB SOURCES "src/*.cpp" "src/*.c" src/main.cpp)
file(GLOB HEADERS "include/*.h" "include/*.hpp")

if(WIN32)
        link_directories(${CMAKE_SOURCE_DIR}/lib)
endif()


add_library(STB_IMAGE libs/stb_image.cpp)
if(MSVC)
else()
        set_source_files_properties(libs/stb_image.cpp include/stb_image.h
                PROPERTIES
                COMPILE_FLAGS
                "-Wno-shift-negative-value -Wno-implicit-fallthrough")
endif(MSVC)        

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_SOURCE_DIR}/cmake/modules")
find_package(GLM REQUIRED)
find_package(GLFW3 REQUIRED)
find_package(ASSIMP REQUIRED)

# Static libraries
add_subdirectory(libs/glad)
add_subdirectory(libs/imgui)

# Grouping libs
if(WIN32)
        set(LIBS glad glfw3 opengl32 assimp STB_IMAGE imgui)
else()
        find_package(OpenGL REQUIRED)
        add_definitions(${OPENGL_DEFINITIONS})
        find_package(X11 REQUIRED)
        set(LIBS ${GLFW3_LIBRARY} glad OpenGL::GL X11 Xrandr Xinerama Xi Xxf86vm Xcursor dl pthread freetype ${ASSIMP_LIBRARIES} STB_IMAGE imgui)
endif(WIN32)








configure_file(configuration/root_directory.h.in configuration/root_directory.h)
include_directories(${CMAKE_BINARY_DIR}/configuration)


include_directories("${CMAKE_SOURCE_DIR}/include")
add_executable(${PROJECT_NAME}
        ${SOURCES})

target_link_libraries(${PROJECT_NAME} ${LIBS})

# set_target_properties(${PROJECT_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin/${PROJECT_NAME}")
set_target_properties(${PROJECT_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin")
file(GLOB SHADERS "resources/shaders/*.vs"
        "resources/shaders/*.fs")
foreach(SHADER ${SHADERS})
    #file(COPY ${SHADER} DESTINATION ${CMAKE_SOURCE_DIR}/bin/shaders)
    watch(${SHADER})
endforeach()

