var SearchParams = new Object();

function Create() {
    $("#CribCreateForm").submit();
}

function Edit() {
    $("#CribEditForm").submit();
}

function InitializeSearchParams() {
    SearchParams.MinPrice = $("#MinPrice").val();
    SearchParams.MaxPrice = $("#MaxPrice").val();
    SearchParams.NumberOfRooms = $("#NumberOfRooms").val();
    SearchParams.Address = $("#Address").val();
    SearchParams.KeyWord = $("#KeyWord").val();
}

function Preview(input) {
    if (input.files && input.files[0]) {
        var previewHolder = "#ImagePreview_" + input.id;
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewHolder).attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

