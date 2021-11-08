﻿/// <binding AfterBuild="default" />

var gulp = require("gulp"),
	merge = require("merge-stream");

var nodeRoot = "./node_modules/";
var targetPath = "./wwwroot/lib/";

gulp.task("default", function () {
	var streams = [
		gulp.src(nodeRoot + "bootstrap/dist/**/*").pipe(gulp.dest(targetPath + "/bootstrap/dist")),
		gulp.src(nodeRoot + "@microsoft/signalr/dist/browser/**/*").pipe(gulp.dest(targetPath + "/signalr/dist")),
		gulp.src(nodeRoot + "tinymce/**/*").pipe(gulp.dest(targetPath + "/tinymce")),
		gulp.src(nodeRoot + "vue/dist/**/*").pipe(gulp.dest(targetPath + "/vue/dist")),
		gulp.src(nodeRoot + "vue-router/dist/**/*").pipe(gulp.dest(targetPath + "/vue-router/dist")),
		gulp.src(nodeRoot + "axios/dist/**/*").pipe(gulp.dest(targetPath + "/axios/dist")),
		gulp.src(nodeRoot + "@popworldmedia/popforums/**/*").pipe(gulp.dest(targetPath + "/PopForums"))
	];
	return merge(streams);
});
