const Email = ref(null);
const PasswordHash = ref(null);
const LastName = ref(null);
const Name = ref(null);
const Phone = ref(null);

const Register = async (event) => {
    event.preventDefault();

    if (!$("#registerModalForm").valid())
        return;

    const data = {
        Email: Email.value,
        PasswordHash: PasswordHash.value,
        LastName: LastName.value,
        Name: Name.value,
        Phone: Phone.value
    };

    try {
        const results = await fetchWithParams("User/Register", data, "POST");
        console.log(results);
    } catch (error) {
        console.error('Error:', error);
    }
}

export default {
    Email,
    PasswordHash,
    LastName,
    Name,
    Phone,
    Register
}



