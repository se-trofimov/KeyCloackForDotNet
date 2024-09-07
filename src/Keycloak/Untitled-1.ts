type Result<T> =
  | { success: true; value: T }
  | { success: false; error: string };

function parseNumber(res: Result<number>) {
  if (res.success) {
    console.log(res.value);
  } else {
    console.log(res.error);
  }
}
