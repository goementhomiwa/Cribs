function PreviewImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        var img_holder = input.siblings("img");
        reader.onload = function (e) {
            img_holder.attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

