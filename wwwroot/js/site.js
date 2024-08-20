// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const {
    ref,
    createApp,
    reactive,
    onMounted,
    defineAsyncComponent
} = Vue;

const isEmptyObject = (obj) => {
    return Object.keys(obj).length === 0;
};