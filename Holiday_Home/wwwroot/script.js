const holidayhomeUri = "api/holidayhome";
let holidayhomes = null;

$(document).ready(function () {
  $.ajax({
    type: "GET",
      url: "api/holidayowner"
    })

  getHolidayhomeData();
});

function getHolidayhomeCount(data) {
  const element = $("#counter");
  if (data) {
    element.text("You have " + data + " holidays");
  } 
  else {
    element.text("No " + name);
  }
}


function getHolidayhomeData() {
  $.ajax({
    type: "GET",
    url: holidayhomeUri,
    cache: false,
    success: function(data) {
      const tBody = $("#holiday-homes");

      $(tBody).empty();

      getHolidayhomeCount(data.length);

      $.each(data, function(key, item) {
        const tr = $("<tr></tr>")
          .append($("<td></td>").text(item.id))
          .append($("<td></td>").text(item.address))
          .append($("<td></td>").text(item.rentalPrice))
          .append($("<td></td>").text(item.homeOwnerId))
          .append(
            $("<td></td>").append(
                $("<button type=\"submit\" class=\"btn btn-success\">Edit</button>").on("click", function() {
                editItem(item.id);
              })
            )
          )
          .append(
            $("<td></td>").append(
                $("<button type=\"submit\" class=\"btn btn-danger\">Delete</button>").on("click", function() {
                deleteItem(item.id);
              })
            )
          );

        tr.appendTo(tBody);
      });

      holidayhomes = data;
    }
  });
}

function addItem() {
  const item = {
    address: $("#add-address").val(),
    rentalPrice: $("#add-price").val(),
    homeOwnerId: $("#add-owner").val(),
  };

  $.ajax({
    type: "POST",
    accepts: "application/json",
    url: holidayhomeUri,
    contentType: "application/json",
    data: JSON.stringify(item),
    error: function(jqXHR, textStatus, errorThrown) {
      alert("Something went wrong!");
    },
    success: function(result) {
      getHolidayhomeData();
      $("#add-address").val("");
      $("#add-price").val("");
      $("#add-owner").val("");
    }
  });
}

function deleteItem(id) {
  $.ajax({
    url: holidayhomeUri + "/" + id,
    type: "DELETE",
    success: function(result) {
      getHolidayhomeData();
    }
  });
}

function editItem(id) {
  $.each(holidayhomes, function(key, item) {
    if (item.id === id) {
      $("#edit-id").val(item.id);
      $("#edit-address").val(item.address);
      $("#edit-price").val(item.rentalPrice);
      $("#edit-owner").val(item.homeOwnerId);
    }
  });
  $("#spoiler").css({ display: "block" });
}

$(".my-form").on("submit", function() {
  const item = {
    address: $("#edit-address").val(),
    rentalPrice: $("#edit-price").val(),
    homeOwnerId: $("#edit-owner").val(),
    id: $("#edit-id").val()
  };

  $.ajax({
    url: holidayhomeUri + "/" + $("#edit-id").val(),
    type: "PUT",
    accepts: "application/json",
    contentType: "application/json",
    data: JSON.stringify(item),
    success: function(result) {
      getHolidayhomeData();
    }
  });

  closeInput();
  return false;
});

function closeInput() {
  $("#spoiler").css({ display: "none" });
}