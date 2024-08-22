const registerOption = defineAsyncComponent(async () => {
    return {
        template: await loadTemplate("Template/_Register"),
        setup() {
            const email = ref('');
            const passwordHash = ref('');
            const lastName = ref('');
            const name = ref('');
            const phone = ref('');

            const showModel = async () => {                
                $("#registerModal").modal("show");
                $.validator.unobtrusive.parse($("#registerModal"));
            }

            const register = async () => {
                if (!$("#registerModalForm").valid())
                    return;

                const data = {
                    Email: email.value,
                    PasswordHash: passwordHash.value,
                    ConfirmPasswordHash: passwordHash.value,
                    LastName: lastName.value,
                    Name: name.value,
                    Phone: phone.value
                };

                try {
                    const results = await fetchWithParams("User/Register", data, "POST");
                    console.log(results);
                } catch (error) {
                    console.error('Error:', error);
                }
            };

            return {
                email,
                passwordHash,
                lastName,
                name,
                phone,
                register,
                showModel
            };
        }
    }
});

export default registerOption;