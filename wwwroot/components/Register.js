﻿const registerOption = defineAsyncComponent(async () => {
    return {
        template: await API.GetTemplate("/Template/_Register"),
        props: {
            onLoggedIn: {
                type: Function,
                required: true
            }
        },
        setup(props) {
            const email = ref('');
            const passwordHash = ref('');
            const confirmPasswordHash = ref('');
            const lastName = ref('');
            const name = ref('');
            const phone = ref('');
            const errorMsg = ref('');

            const clear = () => {
                email.value = '';
                passwordHash.value = '';
                confirmPasswordHash.value = '';
                lastName.value = '';
                name.value = '';
                phone.value = '';
                errorMsg.value = '';
            }

            const showModel = async () => {    
                clear();
                $("#registerModal").modal("show");
                $.validator.unobtrusive.parse($("#registerModal"));
            }

            const register = async () => {
                if (!$("#registerModalForm").valid())
                    return;

                const form = document.getElementById('registerModalForm');
                const formData = new FormData(form);

                const data = {};
                formData.forEach((value, key) => {
                    let keyreplace = key.replace('Register.', '');
                    if (keyreplace !== 'AntiforgeryToken') { // 排除不需要的欄位
                        data[keyreplace] = value;
                    }
                });

                //const data = {
                //    Email: email.value,
                //    PasswordHash: passwordHash.value,
                //    ConfirmPasswordHash: confirmPasswordHash.value,
                //    LastName: lastName.value,
                //    Name: name.value,
                //    Phone: phone.value
                //};

                try {
                    const results = await API.POST("User/Register", data);

                    if (results && results.state) {
                        clear();
                        $("#registerModal").modal("hide");
                        props.onLoggedIn();
                    } else {
                        errorMsg.value = results.errorMsg;
                        passwordHash.value = '';
                    }

                } catch (error) {
                    console.error('Error:', error);
                }
            };

            return {
                email,
                passwordHash,
                confirmPasswordHash,
                lastName,
                name,
                phone,
                register,
                showModel,
                errorMsg
            };
        }
    }
});

export default registerOption;